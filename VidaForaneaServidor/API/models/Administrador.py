from sqlalchemy import null
from extensions import db

lista_administradores = []

class Administrador(db.Model):
    __tablename__ = 'Administrador'

    id = db.Column(db.Integer, primary_key=True)
    nombre = db.Column(db.String(80), nullable=False)
    usuario = db.Column(db.String(200), nullable=False, unique=True)
    contrasenia = db.Column(db.String(200), nullable=False)
    estado = db.Column(db.Boolean(), default=True)

    #recipes = db.relationship('Recipe', backref='user')

    @classmethod
    def get_by_nombre(cls, nombre):
        return cls.query.filter_by(nombre=nombre).first()

    @classmethod
    def get_by_id(cls, id):
        return cls.query.filter_by(id=id).first()

    @classmethod
    def get_by_credentials(cls, usuario, contrasenia):
        return cls.query.filter_by(usuario=usuario, contrasenia=contrasenia).first()

    @classmethod
    def get_by_usuario(cls, usuario):
        return cls.query.filter_by(usuario=usuario).first()

    @classmethod
    def get_all_administradores(cls):
        return cls.query.all()

    def save(self):
        db.session.add(self)
        db.session.commit()
