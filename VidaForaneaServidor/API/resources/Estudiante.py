from tabnanny import check
from flask import request
from flask_restful import Resource
from http import HTTPStatus

from utils import check_password, hash_password
from models.Estudiante import Estudiante, lista_estudiantes


class ListaEstudiantes(Resource):

    def get(self):
        data = []
        estudiantes = Estudiante.get_all_estudiantes()
        for estudiante in estudiantes:
            if estudiante.estado is True:
                data.append({
                    'id': estudiante.id,
                    'nombre': estudiante.nombre,
                    'matricula': estudiante.matricula,
                    'contrasenia': estudiante.contrasenia
                })
        return {'data': data}, HTTPStatus.OK

    def post(self):
        json_data = request.get_json()

        nombre = json_data.get('nombre')
        matricula = json_data.get('matricula')
        contraseniaNoHasheada = json_data.get('contrasenia')
        licenciatura = json_data.get('licenciatura')
        estado = True
        if Estudiante.get_by_matricula(matricula):
            return {'message': 'Estudiante ya registrado'}, HTTPStatus.BAD_REQUEST

        password = hash_password(contraseniaNoHasheada)

        estudiante = Estudiante(
            nombre=nombre,
            matricula=matricula,
            contrasenia=password,
            licenciatura=licenciatura,
            estado=estado
        )
        lista_estudiantes.append(estudiante.nombre)
        estudiante.save()

        data = {
            'id': estudiante.id,
            'nombre': estudiante.nombre,
            'matricula': estudiante.matricula
        }

        return data, HTTPStatus.CREATED

class Login(Resource):
    def post(self, matricula):
        json_data = request.get_json()
        estudiante = Estudiante.get_by_matricula(matricula)
        contraseniaNoHasheada = json_data.get('contrasenia')
        if check_password(contraseniaNoHasheada, estudiante.contrasenia) is False:
            return {'message': 'Error en las credenciales'}, HTTPStatus.NOT_FOUND
        data = {
            'id': estudiante.id,
            'nombre': estudiante.nombre,
            'matricula': estudiante.matricula,
            'licenciatura': estudiante.licenciatura
        }
        return data, HTTPStatus.OK

class RecursoEstudiante(Resource):

    def get(self, estudiante_id):
        estudiante = Estudiante.get_by_id(estudiante_id)
        if estudiante is None:
            return {'message': 'Estudiante no encontrado'}, HTTPStatus.NOT_FOUND
        data = {
            'id': estudiante.id,
            'nombre': estudiante.nombre,
            'matricula': estudiante.matricula,
            'licenciatura': estudiante.licenciatura,
        }
        return data, HTTPStatus.OK

    def patch(self, estudiante_id):
        estudiante = Estudiante.get_by_id(estudiante_id)
        if estudiante is None:
            return {'message': 'Estudiante no encontrado'}, HTTPStatus.NOT_FOUND
        if estudiante.estado is True:
            estudiante.estado = False
        else:
            estudiante.estado = True
        data = {
            'id': estudiante.id,
            'nombre': estudiante.nombre,
            'matricula': estudiante.matricula,
            'estado nuevo': estudiante.estado
        }
        estudiante.save()
        return data, HTTPStatus.OK
