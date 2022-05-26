from tabnanny import check
from flask import request
from flask_restful import Resource
from http import HTTPStatus

from utils import check_password, hash_password
from models.Administrador import Administrador, lista_administradores


class ListaAdministradores(Resource):

    def get(self):
        data = []
        administradores = Administrador.get_all_estudiantes()
        for administrador in administradores:
            if administrador.estado is True:
                data.append({
                    'id': administrador.id,
                    'nombre': administrador.nombre,
                    'usuario': administrador.usuario,
                    'contrasenia': administrador.contrasenia
                })
        return {'data': data}, HTTPStatus.OK

    def post(self):
        json_data = request.get_json()

        nombre = json_data.get('nombre')
        usuario = json_data.get('usuario')
        contraseniaNoHasheada = json_data.get('contrasenia')
        estado = True
        if Administrador.get_by_usuario(usuario):
            return {'message': 'Administrador ya registrado'}, HTTPStatus.BAD_REQUEST

        password = hash_password(contraseniaNoHasheada)

        administrador = Administrador(
            nombre=nombre,
            usuario=usuario,
            contrasenia=password,
            estado=estado
        )
        lista_administradores.append(administrador.nombre)
        administrador.save()

        data = {
            'id': administrador.id,
            'nombre': administrador.nombre,
            'usuario': administrador.usuario
        }

        return data, HTTPStatus.CREATED

class LoginAdmin(Resource):
    def post(self, usuario):
        json_data = request.get_json()
        administrador = Administrador.get_by_usuario(usuario)
        contraseniaNoHasheada = json_data.get('contrasenia')
        if check_password(contraseniaNoHasheada, administrador.contrasenia) is False:
            return {'message': 'Error en las credenciales'}, HTTPStatus.NOT_FOUND
        data = {
            'id': administrador.id,
            'nombre': administrador.nombre,
            'usuario': administrador.usuario,
        }
        return data, HTTPStatus.OK

class RecursoAdministrador(Resource):

    def get(self, administrador_id):
        administrador = Administrador.get_by_id(administrador_id)
        if administrador is None:
            return {'message': 'Estudiante no encontrado'}, HTTPStatus.NOT_FOUND
        data = {
            'id': administrador.id,
            'nombre': administrador.nombre,
            'usuario': administrador.usuario,
        }
        return data, HTTPStatus.OK
