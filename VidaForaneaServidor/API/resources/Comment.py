from tabnanny import check
from flask import request
from flask_restful import Resource
from http import HTTPStatus
from flask_jwt_extended import get_jwt_identity, jwt_required
from models.Comment import Comment, lista_comments
from models.Student import Student


class ListComments(Resource):

    def get(self, forum_id):
        data = []
        comments = Comment.get_by_id_forum(forum_id)
        for comment in comments:
            data.append({
                'id': comment.id,
                'student': comment.student,
                'date': comment.date,
                'hour': comment.hour,
                'description': comment.description,
                'id_forum': comment.id_forum
            })
        return {'data': data}, HTTPStatus.OK

    @jwt_required
    def post(self, forum_id):
        json_data = request.get_json()

        student = json_data.get('student')
        date = json_data.get('date')
        hour = json_data.get('hour')
        description = json_data.get('description')
        id_forum = json_data.get('id_forum')

        if Comment.get_by_id_forum_student(forum_id,student):
            return {'message': 'Comentario ya registrado'}, HTTPStatus.BAD_REQUEST


        comment = Comment(
            student= student,
            date=date,
            hour=hour,
            description=description,
            id_forum=id_forum
        )
        lista_comments.append(comment)
        comment.save()

        data = {
            'id': comment.id,
            'student': comment.student,
            'date': comment.date,
            'hour': comment.hour,
            'description': comment.description,
            'id_forum': comment.id_forum
        }

        return data, HTTPStatus.CREATED



class ResourceComments(Resource):

    def get(self, comment_id, forum_id):
        comment = Comment.get_by_id(comment_id)
        if comment is None:
            return {'message': 'Comentario no encontrado'}, HTTPStatus.NOT_FOUND
        data = {
            'id': comment.id,
            'student': comment.student,
            'date': comment.date,
            'hour': comment.hour,
            'description': comment.description,
            'id_forum': comment.id_forum
        }
        return data, HTTPStatus.OK

    @jwt_required
    def delete(self, comment_id, forum_id):       
        comment = Comment.get_by_id(comment_id)
        if comment is None:
            return {'message': 'Comentario no encontrado'}, HTTPStatus.NOT_FOUND
        current_user = get_jwt_identity()
        current_student = Student.get_by_enrollment(current_user)
        if current_student.rol == 'estudiante'  :
            return {'message': 'Access is not allowed'}, HTTPStatus.FORBIDDEN
        comment.delete()
        return  HTTPStatus.NO_CONTENT
