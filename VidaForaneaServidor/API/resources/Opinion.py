from tabnanny import check
from flask import request
from flask_restful import Resource
from http import HTTPStatus
from flask_jwt_extended import get_jwt_identity, jwt_required

from models.Opinion import Opinion, lista_opinions
from models.Student import Student


class ListOpinions(Resource):

    def get(self, id_place):
        data = []
        opinions = Opinion.get_by_id_place(id_place)
        for opinion in opinions:
            data.append({
                'id': opinion.id,
                'user': opinion.user,
                'date': opinion.date,
                'hour': opinion.hour,
                'score': opinion.score,
                'description': opinion.description,
                'id_place': opinion.id_place
            })
        return {'data': data}, HTTPStatus.OK

    @jwt_required
    def post(self, id_place):
        json_data = request.get_json()

        user = json_data.get('user')
        date = json_data.get('date')
        hour = json_data.get('hour')
        score = json_data.get('score')
        description = json_data.get('description')
        id_place = json_data.get('id_place')

        if Opinion.get_by_id_place_user(id_place,user):
            return {'message': 'Opinion ya registrada'}, HTTPStatus.BAD_REQUEST


        opinion = Opinion(
            user= user,
            date=date,
            hour=hour,
            score=score,
            description=description,
            id_place=id_place
        )
        lista_opinions.append(opinion)
        opinion.save()

        data = {
            'id': opinion.id,
            'user': opinion.user,
            'date': opinion.date,
            'hour': opinion.hour,
            'score': opinion.score,
            'description': opinion.description,
            'id_place': opinion.id_place
        }

        return data, HTTPStatus.CREATED



class ResourceOpinion(Resource):

    def get(self, opinion_id, id_place):
        opinion = Opinion.get_by_id(opinion_id)
        if opinion is None:
            return {'message': 'Opinion no encontrado'}, HTTPStatus.NOT_FOUND
        data = {
            'id': opinion.id,
            'user': opinion.user,
            'date': opinion.date,
            'hour': opinion.hour,
            'score': opinion.score,
            'description': opinion.description,
            'id_place': opinion.id_place
        }
        return data, HTTPStatus.OK
        
    @jwt_required
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
