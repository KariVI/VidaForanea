from flask import Flask
from flask_migrate import Migrate
from flask_restful import Api

from config import Config
from extensions import db
from resources.Comment import ListComments, ResourceComments
from resources.Forum import ListForums, ResourceForum
from resources.Student import ListStudents, ResourceStudent, Login
from resources.Place import ListPlaces, ResourcePlace
from resources.Opinion import ListOpinions, ResourceOpinion

def create_app():
    app = Flask(__name__)
    app.config.from_object(Config)

    register_extensions(app)
    register_resources(app)

    return app


def register_extensions(app):
    db.init_app(app)
    migrate = Migrate(app, db)


def register_resources(app):
    api = Api(app)

    api.add_resource(ListStudents, '/estudiantes')
    api.add_resource(ResourceStudent, '/estudiantes/<string:enrollment>')
    api.add_resource(Login, '/login/<string:enrollment>'),
    api.add_resource(ListPlaces, '/lugares'),
    api.add_resource(ResourcePlace, '/lugares/<int:place_id>')
    api.add_resource(ListOpinions, '/lugares/<int:id_place>/opiniones')
    api.add_resource(ResourceOpinion, '/lugares/<int:id_place>/opiniones/<int:opinion_id>')
    api.add_resource(ListForums, '/foros'),
    api.add_resource(ResourceForum, '/foros/<int:forum_id>')
    api.add_resource(ListComments, '/foros/<int:forum_id>/comentarios')
    api.add_resource(ResourceComments, '/foros/<int:forum_id>/comentarios/<int:comment_id>')

if __name__ == '__main__':
    app = create_app()
    app.run(host='0.0.0.0', port=9090)
