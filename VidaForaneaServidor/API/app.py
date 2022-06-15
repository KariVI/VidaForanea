from flask import Flask
from flask_migrate import Migrate
from flask_restful import Api

from config import Config
from extensions import db,jwt
from resources.Comment import ListComments, ResourceComments
from resources.Forum import ListForums, ResourceForum
from resources.Student import ListStudents, ResourceStudent
from resources.Place import ListPlaces, ListPlacesStatus, ListPlacesType, ResourcePlace
from resources.Opinion import ListOpinions, ResourceOpinion
from resources.Token import TokenResource, RefreshResource, RevokeResource, block_list

def create_app():
    app = Flask(__name__)
    app.config.from_object(Config)

    register_extensions(app)
    register_resources(app)

    return app


def register_extensions(app):
    db.init_app(app)
    migrate = Migrate(app, db)
    jwt.init_app(app)

    @jwt.token_in_blocklist_loader
    def check_if_token_in_blocklist(jwt_header, jwt_payload):
        jti = jwt_payload["jti"]
        return jti in block_list


def register_resources(app):
    api = Api(app)

    api.add_resource(ListStudents, '/estudiantes')
    api.add_resource(ResourceStudent, '/estudiantes/<string:enrollment>')
    
    api.add_resource(ListPlaces, '/lugares'),
    api.add_resource(ResourcePlace, '/lugares/<int:place_id>')
    api.add_resource(ListOpinions, '/lugares/<int:id_place>/opiniones')
    api.add_resource(ResourceOpinion, '/lugares/<int:id_place>/opiniones/<int:opinion_id>')
    api.add_resource(ListPlacesStatus, '/lugares/<string:status>'),
    api.add_resource(ListPlacesType, '/lugares/<string:status>/<string:type_place>')

    api.add_resource(ListForums, '/foros'),
    api.add_resource(ResourceForum, '/foros/<int:forum_id>')
    api.add_resource(ListComments, '/foros/<int:forum_id>/comentarios')
    api.add_resource(ResourceComments, '/foros/<int:forum_id>/comentarios/<int:comment_id>'),
    
    api.add_resource(TokenResource, '/token')
    api.add_resource(RefreshResource, '/refresh')
    api.add_resource(RevokeResource, '/revoke')

if __name__ == '__main__':
    app = create_app()
    app.run(host='0.0.0.0', port=5000)
