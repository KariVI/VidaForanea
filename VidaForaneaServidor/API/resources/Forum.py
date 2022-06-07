from tabnanny import check
from xml.dom import ValidationErr
from flask import request
from flask_restful import Resource
from http import HTTPStatus
from flask_jwt_extended import get_jwt_identity, jwt_required
from marshmallow import ValidationError
from flask import jsonify
from models.Forum import Forum, lista_forums
from models.Student import Student
from schemas.Forum import ForumSchema

forum_schema = ForumSchema()
forums_list_schema = ForumSchema(many=True)

class ListForums(Resource):

    def get(self):
        forums = Forum.get_all_forums()  
        return forums_list_schema.dump(forums), HTTPStatus.OK
    
    @jwt_required()
    def post(self):
        json_data = request.get_json()
        degree = json_data.get('degree')
        try:
            data = forum_schema.load(data=json_data)
        except ValidationError as exc:
            return {'message': "Validation errors", 'errors': exc.messages}, HTTPStatus.BAD_REQUEST
        if Forum.get_by_degree(degree):
            return {'message': 'Foro ya registrado'}, HTTPStatus.BAD_REQUEST
        current_user = get_jwt_identity()
        current_student = Student.get_by_enrollment(current_user)
        if current_student.rol == 'estudiante'  :
            return {'message': 'Access is not allowed'}, HTTPStatus.FORBIDDEN      
        forum = Forum(**data)
        lista_forums.append(forum.degree)
        forum.save()
        return  HTTPStatus.CREATED



class ResourceForum(Resource):

    def get(self, forum_id):
        forum = Forum.get_by_id(forum_id)
        if forum is None:
            return {'message': 'Foro no encontrado'}, HTTPStatus.NOT_FOUND       
        return forum_schema.dump(forum), HTTPStatus.OK
