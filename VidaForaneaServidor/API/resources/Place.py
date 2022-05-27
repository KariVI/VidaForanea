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
                'type_place': place.type_place,
                'image': place.image.decode("utf-8")
            })
        return {'data': data}, HTTPStatus.OK

    def post(self):
        json_data = request.get_json()

        name = json_data.get('name')
        address = json_data.get('address')
        services = json_data.get('services')
        schedule = json_data.get('schedule')
        if json_data.get('status') == 1:
            status = "aprobado"
        if json_data.get('status') == 0:
            status = "pendiente"

        type_place=json_data.get('type_place')
        imageNotEncodedToBytes = json_data.get('image')
        image = bytes(imageNotEncodedToBytes, 'utf-8')
        if Place.get_by_name(name):
            return {'message': 'Lugar ya registrada'}, HTTPStatus.BAD_REQUEST

        place = Place(
            name= name,
            address=address,
            services=services,
            schedule=schedule,
            type_place=type_place,
            image = image,
            status = status
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
            'type_place': place.type_place,
            'image': place.image.decode("utf-8")
        }

        return data, HTTPStatus.CREATED


class ResourcePlace(Resource):

    def get(self, place_id):
        place = next((place for place in lista_places if place.id == place_id ), None)
        if place is None:
            return {'message': 'Lugar no encontrado'}, HTTPStatus.NOT_FOUND
        data = {
            'id': place.id,
            'name': place.name,
            'address': place.address,
            'services': place.services,
            'schedule': place.schedule,
            'status': place.status,
            'type_place': place.type_place,
            'image': place.image.decode("utf-8")
        }
        return data, HTTPStatus.OK

    def delete(self, place_id):
        place = Place.get_by_id(place_id)
        if place is None:
            return {'message': 'Lugar no encontrado'}, HTTPStatus.NOT_FOUND
        place.delete()
        return  HTTPStatus.NO_CONTENT

    def put(self, place_id):
        data = request.get_json()
        place = Place.get_by_id(place_id)
        if place is None:
            return {'message': 'Lugar no encontrado'}, HTTPStatus.NOT_FOUND
        statusCheck = data['status']
        if statusCheck == 1:
           place.status= 'aprobado'
        if statusCheck == 0:
           place.status = 'pendiente'
        print(statusCheck)
        place.name = data['name']
        place.address = data['address']
        place.services = data['services']
        place.schedule = data['schedule']
        place.type_place = data['type_place']
        place.image = bytes(data['image'], 'utf-8')
        place.save()
        return data, HTTPStatus.OK

    def patch(self, place_id):
        place = next((place for place in lista_places if place.id == place_id ), None)
        if place is None:
            return {'message': 'Lugar no encontrado'}, HTTPStatus.NOT_FOUND
        json_data = request.get_json()
        name = json_data.get('name')
        address = json_data.get('address')
        services = json_data.get('services')
        schedule = json_data.get('schedule')
        type_place=json_data.get('type_place')
        imageNotEncodedToBytes = json_data.get('image')
        image = bytes(imageNotEncodedToBytes, 'utf-8')
        place.name = name
        place.address = address
        place.services = services
        place.schedule = schedule
        if json_data.get('status') == 1:
            status = "aprobado"
        if json_data.get('status') == 0:
            status = "pendiente"
        place.type_place = type_place
        place.image = image
        
       
        #if place.status =='Pendiente':
        #   place.status='Aprobado'

        data = {
            'id': place.id,
            'name': place.name,
            'address': place.address,
            'services': place.services,
            'schedule': place.schedule,
            'new status': place.status,
            'type_place': place.type_place,
            'image': place.image.decode("utf-8")
        }
        place.save()
        return data, HTTPStatus.OK

class ListPlacesStatus(Resource):

    def get(self,status):
        data = []
        places = Place.get_by_status(status)
        for place in places:
            data.append({
                'id': place.id,
                'name': place.name,
                'address': place.address,
                'services': place.services,
                'schedule': place.schedule,
                'status': place.status,
                'type_place': place.type_place,
                'image': place.image.decode("utf-8")
            })
        return {'data': data}, HTTPStatus.OK

class ListPlacesType(Resource):
    def get(self,status,type_place):
        data = []
        places = Place.get_by_type_place(status,type_place)
        for place in places:
            data.append({
                'id': place.id,
                'name': place.name,
                'address': place.address,
                'services': place.services,
                'schedule': place.schedule,
                'status': place.status,
                'type_place': place.type_place,
                'image': place.image.decode("utf-8")
            })
        return {'data': data}, HTTPStatus.OK
