from sqlalchemy import null
from extensions import db

lista_forums = []

class Forum(db.Model):
    __tablename__ = 'Forum'

    id = db.Column(db.Integer, primary_key=True)
    degree = db.Column(db.String(200), nullable=False)
    


    
    @classmethod
    def get_by_id(cls, id):
        return cls.query.filter_by(id=id).first()

    @classmethod
    def get_by_degree(cls, degree):
        return cls.query.filter_by(degree=degree).all()
    
   
    @classmethod
    def get_all_forums(cls):
        return cls.query.all()
    
  
    def save(self):
        db.session.add(self)
        db.session.commit()