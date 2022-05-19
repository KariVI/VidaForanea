from sqlalchemy import null
from extensions import db

lista_estudiantes = []

class Estudiante(db.Model):
    __tablename__ = 'Estudiante'

    id = db.Column(db.Integer, primary_key=True)
    nombre = db.Column(db.String(80), nullable=False)
    matricula = db.Column(db.String(200), nullable=False, unique=True)
    contrasenia = db.Column(db.String(200), nullable=False)
    estado = db.Column(db.Boolean(), default=True)
    licenciatura = db.Column(db.String(100), nullable=False)

    #recipes = db.relationship('Recipe', backref='user')

    @classmethod
    def get_by_nombre(cls, nombre):
        return cls.query.filter_by(nombre=nombre).first()

    @classmethod
    def get_by_id(cls, id):
        return cls.query.filter_by(id=id).first()

    @classmethod
    def get_by_credentials(cls, matricula, contrasenia):
        return cls.query.filter_by(matricula=matricula, contrasenia=contrasenia).first()

    @classmethod
    def get_by_matricula(cls, matricula):
        return cls.query.filter_by(matricula=matricula).first()

    @classmethod
    def get_all_estudiantes(cls):
        return cls.query.all()

    def save(self):
        db.session.add(self)
        db.session.commit()