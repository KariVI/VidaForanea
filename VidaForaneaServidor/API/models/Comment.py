from sqlalchemy import null
from extensions import db

lista_comments = []

class Comment(db.Model):
    __tablename__ = 'Comment'

    id = db.Column(db.Integer, primary_key=True)
    student=db.Column(db.String(20), db.ForeignKey('Student.enrollment'),
        nullable=False)
    date = db.Column(db.String(200), nullable=False)
    hour = db.Column(db.String(5), nullable=False)
    description = db.Column(db.String(200), nullable=False)
    id_forum = db.Column(db.Integer, db.ForeignKey('Forum.id'),
        nullable=False)


    
    @classmethod
    def get_by_id(cls, id):
        return cls.query.filter_by(id=id).first()

    @classmethod
    def get_by_id_forum(cls, id_forum):
        return cls.query.filter_by(id_forum=id_forum).all()
    
    @classmethod
    def get_by_enrollement_forum_student(cls, id_forum, student):
        return cls.query.filter_by(id_forum=id_forum, student=student).all()
    @classmethod
    def get_all_comments(cls):
        return cls.query.all()
    
    def delete(self):
        db.session.delete(self)
        db.session.commit()

    def save(self):
        db.session.add(self)
        db.session.commit()