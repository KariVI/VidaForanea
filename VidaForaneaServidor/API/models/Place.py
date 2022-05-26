from sqlalchemy import null
from extensions import db

lista_places = []

class Place(db.Model):
    __tablename__ = 'Place'

    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(80), nullable=False, unique=True)
    address = db.Column(db.String(500), nullable=False)
    services = db.Column(db.String(500), nullable=False)
    schedule = db.Column(db.String(500), nullable=False)
    status = db.Column(db.String(200), default="pendiente")
    type_place = db.Column(db.String(200), nullable=False)
    image = db.Column(db.BLOB, nullable = False)

    @classmethod
    def get_by_name(cls, name):
        return cls.query.filter_by(name=name).first()

    @classmethod
    def get_by_id(cls, id):
        return cls.query.filter_by(id=id).first()
    @classmethod
    def get_by_status(cls, status):
        return cls.query.filter_by(status=status).all()
    @classmethod
    def get_by_type_place(cls, status, type_place):
        return cls.query.filter_by(type_place=type_place, status=status).all()
    @classmethod
    def get_all_places(cls):
        return cls.query.all()

    def save(self):
        db.session.add(self)
        db.session.commit()

    def delete(self):
        db.session.delete(self)
        db.session.commit()
