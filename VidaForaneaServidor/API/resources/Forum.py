from tabnanny import check
from flask import request
from flask_restful import Resource
from http import HTTPStatus

from models.Forum import Forum, lista_forums


class ListForums(Resource):

    def get(self):
        data = []
        forums = Forum.get_all_forums()
        for forum in forums:
            data.append({
                'id': forum.id,
                'degree': forum.degree,

            })
        return {'data': data}, HTTPStatus.OK

    def post(self):
        json_data = request.get_json()

        degree = json_data.get('degree')
  

        if Forum.get_by_degree(degree):
            return {'message': 'Foro ya registrado'}, HTTPStatus.BAD_REQUEST


        forum = Forum(
            degree=degree
        )
        lista_forums.append(forum)
        forum.save()

        data = {
            'id': forum.id,
            'degree': forum.degree
            
        }

        return data, HTTPStatus.CREATED



class ResourceForum(Resource):

    def get(self, forum_id):
        forum = Forum.get_by_id(forum_id)
        if forum is None:
            return {'message': 'Foro no encontrado'}, HTTPStatus.NOT_FOUND
        data = {
            'id': forum.id,
            'degree': forum.degree         
        }
        return data, HTTPStatus.OK
