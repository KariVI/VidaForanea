
from sqlalchemy import null
from extensions import db

lista_opinions = []

class Opinion(db.Model):
    __tablename__ = 'Opinion'

    id = db.Column(db.Integer, primary_key=True)
    student=db.Column(db.String(200),
        nullable=False)
    date = db.Column(db.String(200), nullable=False)
    hour = db.Column(db.String(5), nullable=False)
    description = db.Column(db.String(200), nullable=False)
    score=db.Column(db.Integer, nullable=False)
    id_place = db.Column(db.Integer, db.ForeignKey('Place.id'),
        nullable=False)



    @classmethod
    def get_by_id(cls, id):
        return cls.query.filter_by(id=id).first()

    @classmethod
    def get_by_id_place(cls, id_place):
        return cls.query.filter_by(id_place=id_place).all()

    @classmethod
    def get_by_id_place_student(cls, id_place, id_student):
        return cls.query.filter_by(id_place=id_place, student=id_student).all()
    @classmethod
    def get_all_opinions(cls):
        return cls.query.all()

    def delete(self):
        db.session.delete(self)
        db.session.commit()

    def save(self):
        db.session.add(self)
        db.session.commit()