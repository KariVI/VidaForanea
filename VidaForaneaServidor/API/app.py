from flask import Flask
from flask_migrate import Migrate
from flask_restful import Api

from config import Config
from extensions import db

from resources.Estudiante import ListaEstudiantes, RecursoEstudiante, Login
from resources.Administrador import ListaAdministradores, RecursoAdministrador, LoginAdmin

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

    api.add_resource(ListaEstudiantes, '/estudiantes')
    api.add_resource(RecursoEstudiante, '/estudiantes/<string:matricula>')
    api.add_resource(Login, '/login/<string:matricula>')
    api.add_resource(ListaAdministradores, '/administradores')
    api.add_resource(RecursoAdministrador, '/administradores/<string:usuario>')
    api.add_resource(LoginAdmin, '/loginAdmin/<string:usuario>')
    #api.add_resource(RecipeListResource, '/recipes')
    #api.add_resource(RecipeResource, '/recipes/<int:recipe_id>')
    #api.add_resource(RecipePublishResource, '/recipes/<int:recipe_id>/publish')


if __name__ == '__main__':
    app = create_app()

    app.run(host='0.0.0.0', port=9090)
