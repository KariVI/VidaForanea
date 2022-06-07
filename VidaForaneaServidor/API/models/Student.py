from email.policy import default
from sqlalchemy import null
from extensions import db

lista_students = []

class Student(db.Model):
    __tablename__ = 'Student'

    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(80), nullable=False)
    enrollment = db.Column(db.String(200), nullable=False, unique=True)
    password = db.Column(db.String(200), nullable=False)
    status = db.Column(db.Boolean(), default=True)
    degree = db.Column(db.String(100), nullable=False)
    rol = db.Column(db.String(200), nullable=False, default="estudiante")

    @classmethod
    def get_by_name(cls, name):
        return cls.query.filter_by(name=name).first()

    @classmethod
    def get_by_id(cls, id):
        return cls.query.filter_by(id=id).first()

    @classmethod
    def get_by_credentials(cls, enrollment, password):
        return cls.query.filter_by(enrollment=enrollment, password=password).first()

    @classmethod
    def get_by_enrollment(cls, enrollment):
        return cls.query.filter_by(enrollment=enrollment).first()

    @classmethod
    def get_all_students(cls):
        return cls.query.all()

    def save(self):
        db.session.add(self)
        db.session.commit()