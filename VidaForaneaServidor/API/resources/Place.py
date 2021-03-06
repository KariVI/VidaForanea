import json
from tabnanny import check
from flask import request
from flask_restful import Resource
from http import HTTPStatus
from flask_jwt_extended import get_jwt_identity, jwt_required
from marshmallow import ValidationError
from flask import jsonify

from models.Place import Place, lista_places
from models.Student import Student
from schemas.Place import PlaceSchema

place_schema = PlaceSchema()
places_list_schema = PlaceSchema(many=True)

class ListPlaces(Resource):
    @jwt_required()
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

    @jwt_required()
    def post(self):
        json_data = request.get_json()

        name = json_data.get('name')
        status = ""
        if json_data.get('status') == 1:
            status = "aprobado"
        if json_data.get('status') == 0:
            status = "pendiente"
        image_not_encoded = json_data.get('image')
        image = bytes(image_not_encoded, 'utf-8')
        json_data["image"] = image
        json_data["status"] = status
        try:
            data = place_schema.load(data=json_data)
        except ValidationError as exc:
            return {'message': "Validation errors", 'errors': exc.messages}, HTTPStatus.BAD_REQUEST
        if Place.get_by_name(name):
            return {'message': 'Place already registered'}, HTTPStatus.BAD_REQUEST
        place = Place(**data)
        lista_places.append(place)
        place.save()
        response=jsonify({'id': place.id,
            'name': place.name,
            'address': place.address,
            'services': place.services,
            'schedule': place.schedule,
            'status': place.status,
            'type_place': place.type_place})
        response.status_code=201
        return  response


class ResourcePlace(Resource):
    @jwt_required()
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

    @jwt_required()
    def delete(self, place_id):
        place = Place.get_by_id(place_id)
        if place is None:
            return {'message': 'Lugar no encontrado'}, HTTPStatus.NOT_FOUND
        current_user = get_jwt_identity()
        current_student = Student.get_by_enrollment(current_user)
        if current_student.rol == 'estudiante' or current_user=='moderador' :
            response=jsonify({'message': 'Access is not allowed'})
            response.status_code=403
            return response
        place.delete()
        response=jsonify({})
        response.status_code=204
        return  response

    @jwt_required()
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
        place.name = data['name']
        place.address = data['address']
        place.services = data['services']
        place.schedule = data['schedule']
        place.type_place = data['type_place']
        place.image = bytes(data['image'], 'utf-8')
        current_user = get_jwt_identity()
        current_student = Student.get_by_enrollment(current_user)
        if current_student.rol == 'estudiante' or current_user=='moderador' :
            response=jsonify({'message': 'Access is not allowed'})
            response.status_code=403
            return response
        place.save()
        response=jsonify({'id': place.id,
            'name': place.name,
            'address': place.address,
            'services': place.services,
            'schedule': place.schedule,
            'status': place.status,
            'type_place': place.type_place})
        response.status_code=200
        return  response

class ListPlacesStatus(Resource):
    @jwt_required()
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
    @jwt_required()
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