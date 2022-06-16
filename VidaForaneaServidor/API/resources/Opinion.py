from tabnanny import check
from flask import request
from flask_restful import Resource
from http import HTTPStatus
from flask_jwt_extended import get_jwt_identity, jwt_required
from marshmallow import ValidationError

from models.Opinion import Opinion, lista_opinions
from models.Student import Student
from schemas.Opinion import OpinionSchema
from flask import jsonify

opinion_schema = OpinionSchema()
opinion_list_schema = OpinionSchema(many=True)

class ListOpinions(Resource):
    @jwt_required()
    def get(self, id_place):
        opinions = Opinion.get_by_id_place(id_place)

        return opinion_list_schema.dump(opinions), HTTPStatus.OK

    @jwt_required()
    def post(self, id_place):
        json_data = request.get_json()
        student = json_data.get('student')       
        try:
            data = opinion_schema.load(data=json_data)
        except ValidationError as exc:
            return {'message': "Validation errors", 'errors': exc.messages}, HTTPStatus.BAD_REQUEST
        if Opinion.get_by_id_place_student(id_place,student):
            return {'message': 'Opinion ya registrada'}, HTTPStatus.BAD_REQUEST
        opinion = Opinion(**data)
        lista_opinions.append(opinion)
        opinion.save()
        response=opinion_schema.dump(opinion)
        response.status_code=201
        return  response



class ResourceOpinion(Resource):
    @jwt_required()
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
        if current_student.rol == 'estudiante'  and current_user != opinion.student:
            response=jsonify({'message': 'Access is not allowed'})
            response.status_code=403
            return response
        opinion.delete()
        response=jsonify({})
        response.status_code=204
        return  response
