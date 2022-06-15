from tabnanny import check
from flask import request
from flask_restful import Resource
from http import HTTPStatus
from flask_jwt_extended import get_jwt_identity, jwt_required
from marshmallow import ValidationError
from models.Comment import Comment, lista_comments
from models.Student import Student
from schemas.Comment import CommentSchema
from flask import jsonify

comment_schema = CommentSchema()
comments_list_schema = CommentSchema(many=True)

class ListComments(Resource):
    @jwt_required()
    def get(self, forum_id):
        data = []
        comments = Comment.get_by_forum_id(forum_id)
        return comments_list_schema.dump(comments), HTTPStatus.OK

    @jwt_required()
    def post(self, forum_id):
        json_data = request.get_json()

        student = json_data.get('student')
        forum_id = json_data.get('forum_id')
        try:
            data = comment_schema.load(data=json_data)
        except ValidationError as exc:
            return {'message': "Validation errors", 'errors': exc.messages}, HTTPStatus.BAD_REQUEST
        if Comment.get_by_forum_id_student(forum_id,student):
            return {'message': 'Comentario ya registrado'}, HTTPStatus.BAD_REQUEST


        comment = Comment(**data)
        lista_comments.append(comment)
        comment.save()
        response=comment_schema.dump(comment)
        response.status_code=201
        return  response



class ResourceComments(Resource):
    @jwt_required()
    def get(self, comment_id, forum_id):
        comment = Comment.get_by_id(comment_id)
        if comment is None:
            return {'message': 'Comentario no encontrado'}, HTTPStatus.NOT_FOUND
        return comment_schema.dump(comment), HTTPStatus.OK

    @jwt_required()
    def delete(self, comment_id, forum_id):       
        comment = Comment.get_by_id(comment_id)
        if comment is None:
            return {'message': 'Comentario no encontrado'}, HTTPStatus.NOT_FOUND
        current_user = get_jwt_identity()
        current_student = Student.get_by_enrollment(current_user)
        if current_user==comment.student or current_student.rol != 'estudiante'  :
            comment.delete()
            response=jsonify({})
            response.status_code=HTTPStatus.NO_CONTENT
            return response
        else:
            response=jsonify({'message': 'Access is not allowed'})
            response.status_code=403
            return response
        
  
