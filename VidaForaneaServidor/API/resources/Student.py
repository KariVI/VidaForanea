from tabnanny import check
from flask import request
from flask_restful import Resource
from http import HTTPStatus
from flask_jwt_extended import get_jwt_identity
from marshmallow import ValidationError

from utils import check_password, hash_password
from models.Student import Student, lista_students
from schemas.Student import StudentSchema

student_schema = StudentSchema()
students_list_schema = StudentSchema(many=True)
class ListStudents(Resource):

    def get(self):
        data = []
        students = Student.get_all_students()
        return students_list_schema.dump(students), HTTPStatus.OK

    def post(self):
        json_data = request.get_json()
        try:
            data = student_schema.load(data=json_data)
        except ValidationError as exc:
            return {'message': "Validation errors", 'errors': exc.messages}, HTTPStatus.BAD_REQUEST      
        enrollment = json_data.get('enrollment')       
        if Student.get_by_enrollment(enrollment):
            return {'message': 'Estudiante ya registrado'}, HTTPStatus.BAD_REQUEST
        student = Student(**data)
        lista_students.append(student.enrollment)
        student.save()
        return student_schema.dump(student), HTTPStatus.CREATED


    
class ResourceStudent(Resource):

    def get(self, enrollment):
        student = Student.get_by_enrollment(enrollment)
        if student is None:
            
            return {'message': 'Estudiante no encontrado'}, HTTPStatus.NOT_FOUND
        
        return student_schema.dump(student), HTTPStatus.OK

    def patch(self, enrollment):
        student = Student.get_by_enrollment(enrollment)
        if student is None:
            return {'message': 'Estudiante no encontrado'}, HTTPStatus.NOT_FOUND
        if student.status is True:
            student.status = False
        else:
            student.status = True
    
        student.save()
        return student_schema.dump(student), HTTPStatus.OK



