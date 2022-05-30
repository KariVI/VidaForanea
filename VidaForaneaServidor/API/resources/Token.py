from http import HTTPStatus
from traceback import print_last
from flask import request
from flask_restful import Resource
from flask_jwt_extended import create_access_token
from flask_jwt_extended import create_refresh_token
from flask_jwt_extended import get_jwt
from flask_jwt_extended import get_jwt_identity
from flask_jwt_extended import jwt_required
from flask_jwt_extended import JWTManager

from utils import check_password
from models.Student import Student

block_list = set()


class TokenResource(Resource):


    def post(self):

        json_data = request.get_json()

        enrollment = json_data.get('enrollment')
        password = json_data.get('password')

        student = Student.get_by_enrollment(enrollment)
        print("Matricula" )
        print(student.enrollment)
        if not student or not check_password(password, student.password):
            return {'message': 'enrollment or password is incorrect'}, HTTPStatus.UNAUTHORIZED

        access_token = create_access_token(identity=student.enrollment, fresh=True)
        refresh_token = create_refresh_token(identity=student.enrollment)

        return {'access_token': access_token, 'refresh_token': refresh_token}, HTTPStatus.OK

   

class RefreshResource(Resource):


    @jwt_required(refresh=True)
    def post(self):
        current_user = get_jwt_identity()

        token = create_access_token(identity=current_user, fresh=False)

        return {'token': token}, HTTPStatus.OK


class RevokeResource(Resource):

    @jwt_required()
    def post(self):
        jti = get_jwt()['jti']

        block_list.add(jti)

        return {'message': 'Successfully logged out'}, HTTPStatus.OK