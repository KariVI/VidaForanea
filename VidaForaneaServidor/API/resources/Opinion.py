from tabnanny import check
from flask import request
from flask_restful import Resource
from http import HTTPStatus
from flask_jwt_extended import get_jwt_identity, jwt_required
from marshmallow import ValidationError

from models.Opinion import Opinion, lista_opinions
from models.Student import Student
from schemas.Opinion import OpinionSchema

opinion_schema = OpinionSchema()
opinion_list_schema = OpinionSchema(many=True)

class ListOpinions(Resource):

    def get(self, id_place):
        data = []
        opinions = Opinion.get_by_id_place(id_place)

        return opinion_list_schema.dump(opinions), HTTPStatus.OK

    @jwt_required()
    def post(self, id_place):
        json_data = request.get_json()

        user = json_data.get('user')       
        id_place = json_data.get('id_place')

        try:
            data = opinion_schema.load(data=json_data)
        except ValidationError as exc:
            return {'message': "Validation errors", 'errors': exc.messages}, HTTPStatus.BAD_REQUEST
        if Opinion.get_by_id_place_user(id_place,user):
            return {'message': 'Opinion ya registrada'}, HTTPStatus.BAD_REQUEST


        opinion = Opinion(**data)
        lista_opinions.append(opinion)
        opinion.save()

        return  HTTPStatus.CREATED



class ResourceOpinion(Resource):

    def get(self, opinion_id, id_place):
        opinion = Opinion.get_by_id(opinion_id)
        if opinion is None:
            return {'message': 'Opinion no encontrado'}, HTTPStatus.NOT_FOUND

        return opinion_schema.dump(opinion), HTTPStatus.OK
        
    @jwt_required()
    def delete(self, opinion_id, id_place):       
        opinion = Opinion.get_by_id(opinion_id)

        if opinion is None:
            return {'message': 'Opinion no encontrado'}, HTTPStatus.NOT_FOUND
        current_user = get_jwt_identity()
        current_student = Student.get_by_enrollment(current_user)
        if current_student.rol == 'estudiante'  :
            return {'message': 'Access is not allowed'}, HTTPStatus.FORBIDDEN
        opinion.delete()
        return  HTTPStatus.NO_CONTENT
