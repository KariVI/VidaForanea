from tabnanny import check
from flask import request
from flask_restful import Resource
from http import HTTPStatus

from models.Opinion import Opinion, lista_opinions


class ListOpinions(Resource):

    def get(self, id_place):
        data = []
        opinions = Opinion.get_by_id_place(id_place)
        for opinion in opinions:
            data.append({
                'id': opinion.id,
                'student': opinion.student,
                'date': opinion.date,
                'hour': opinion.hour,
                'description': opinion.description,
                'id_place': opinion.id_place
            })
        return {'data': data}, HTTPStatus.OK

    def post(self, id_place):
        json_data = request.get_json()

        student = json_data.get('student')
        date = json_data.get('date')
        hour = json_data.get('hour')
        description = json_data.get('description')
        id_place = json_data.get('id_place')

        if Opinion.get_by_id_place_student(id_place,student):
            return {'message': 'Opinion ya registrada'}, HTTPStatus.BAD_REQUEST


        opinion = Opinion(
            student= student,
            date=date,
            hour=hour,
            description=description,
            id_place=id_place
        )
        lista_opinions.append(opinion)
        opinion.save()

        data = {
            'id': opinion.id,
            'student': opinion.student,
            'date': opinion.date,
            'hour': opinion.hour,
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
            'student': opinion.student,
            'date': opinion.date,
            'hour': opinion.hour,
            'description': opinion.description,
            'id_place': opinion.id_place
        }
        return data, HTTPStatus.OK

    def delete(self, opinion_id, id_place):       
        opinion = Opinion.get_by_id(opinion_id)

        if opinion is None:
            return {'message': 'Opinion no encontrado'}, HTTPStatus.NOT_FOUND
        opinion.delete()
        return  HTTPStatus.NO_CONTENT
