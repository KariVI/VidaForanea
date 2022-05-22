from tabnanny import check
from flask import request
from flask_restful import Resource
from http import HTTPStatus

from models.Place import Place, lista_places

class ListPlaces(Resource):

    def get(self):
        data = []
        places = Place.get_all_places()
        for place in places:
            data.append({
                'id': place.id,
                'name': place.name,
                'address': place.address,
                'services': place.services,
                'schedule': place.schedule,
                'status': place.status,
                'type_place': place.type_place
            })
        return {'data': data}, HTTPStatus.OK

    def post(self):
        json_data = request.get_json()

        name = json_data.get('name')
        address = json_data.get('address')
        services = json_data.get('services')
        schedule = json_data.get('schedule')
        status="Pendiente"
        type_place=json_data.get('type_place')
        if Place.get_by_name(name):
            return {'message': 'place ya registrada'}, HTTPStatus.BAD_REQUEST

        place = Place(
            name= name,
            address=address,
            services=services,
            schedule=schedule,
            type_place=type_place
        )
        lista_places.append(place)
        place.save()

        data = {
            'id': place.id,
            'name': place.name,
            'address': place.address,
            'services': place.services,
            'schedule': place.schedule,
            'status': place.status,
            'type_place': place.type_place
        }

        return data, HTTPStatus.CREATED


class ResourcePlace(Resource):

    def get(self, place_id):
        place = next((place for place in lista_places if place.id == place_id ), None)
        if place is None:
            return {'message': 'place no encontrado'}, HTTPStatus.NOT_FOUND
        data = {
            'id': place.id,
            'name': place.name,
            'address': place.address,
            'services': place.services,
            'schedule': place.schedule,
            'status': place.status,
            'type_place': place.type_place
        }
        return data, HTTPStatus.OK

    def delete(self, place_id):
        place = Place.get_by_id(place_id)
        if place is None:
            return {'message': 'place no encontrado'}, HTTPStatus.NOT_FOUND
        place.delete()
        return  HTTPStatus.NO_CONTENT

    def put(self, place_id):
        data = request.get_json()
        place = next((place for place in lista_places if place.id == place_id ), None)
        if place is None:
            return {'message': 'place no encontrado'}, HTTPStatus.NOT_FOUND

        if place.status is 'Pendiente':
            place.status= 'Aprobado'
        
        place.name = data['name']
        place.address = data['address']
        place.services = data['services']
        place.schedule = data['schedule']
        place.type_place = data['type_place']


        return data, HTTPStatus.OK
