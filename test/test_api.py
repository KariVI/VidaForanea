
from cgi import test
from http import HTTPStatus
import http.client as httpClient
import json

def test_PostStudent():
    connection = httpClient.HTTPConnection("127.0.0.1",9000)
    data = {
        "name" : "Carlos Miguel Pérez Pérez",
        "enrollment": "zs19014020",
        "password": "12345",
        "status": 1,
        "rol" : "estudiante",
        "degree" : "Lic. en Ingeniería de Software"
    }
    data2 = json.dumps(data)
    headers = {"Content-Type": "application/json"}
    response = connection.request(
        "POST",
        "/estudiantes",
        data2,
        headers,
    )
    generalResponse = connection.getresponse()
    statusCode = generalResponse.status
    assert HTTPStatus.CREATED == statusCode

def test_PostAdmin():
    connection = httpClient.HTTPConnection("127.0.0.1",9000)
    data = {
        "name" : "Admin",
        "enrollment": "admin",
        "password": "12345",
        "status": 1,
        "rol" : "administrador",
        "degree" : "Lic. en Ingeniería de Software"
    }
    data2 = json.dumps(data)
    headers = {"Content-Type": "application/json"}
    response = connection.request(
        "POST",
        "/estudiantes",
        data2,
        headers,
    )
    generalResponse = connection.getresponse()
    statusCode = generalResponse.status
    assert HTTPStatus.CREATED == statusCode

def test_Login():
    global token
    global refresh_token
    connection = httpClient.HTTPConnection("127.0.0.1",9000)
    data = {
        "enrollment": "admin",
        "password": "12345",
    }
    data2 = json.dumps(data)
    headers = {"Content-Type": "application/json"}
    response = connection.request(
        "POST",
        "/token",
        data2,
        headers,
    )
    generalResponse = connection.getresponse()
    readResponse = generalResponse.read()
    json_format = json.loads(readResponse)
    statusCode = generalResponse.status
    token = json_format["access_token"]
    refresh_token = json_format["refresh_token"]
    assert HTTPStatus.OK == statusCode

def test_Refresh():
    global token
    global refresh_token
    connection = httpClient.HTTPConnection("127.0.0.1",9000)
    data = {
        "enrollment": "zs19014017",
        "password": "12345",
    }
    bear = "Bearer " + refresh_token
    data2 = json.dumps(data)
    headers = {"Content-Type": "application/json", "Authorization" : bear}
    response = connection.request(
        "POST",
        "/refresh",
        data2,
        headers,
    )
    generalResponse = connection.getresponse()
    readResponse = generalResponse.read()
    json_format = json.loads(readResponse)
    statusCode = generalResponse.status
    token = json_format["token"]
    assert HTTPStatus.OK == statusCode

def test_GetStudent():
    global token
    connection = httpClient.HTTPConnection("127.0.0.1",9000)
    bear = "Bearer " + token
    headers = {"Content-Type": "application/json", "Authorization" : bear}
    response = connection.request(
        "GET",
        "/estudiantes/zs19014020",
        "",
        headers
    )
    generalResponse = connection.getresponse()
    statusCode = generalResponse.status
    assert statusCode == HTTPStatus.OK

def test_PostPlace():
    global token
    connection = httpClient.HTTPConnection("127.0.0.1",9000)
    data = {
        "name": "Bola de Oro",
        "address": "Calle Centro #25",
        "services" : "Ofrecen cafe muy rico",
        "schedule" : "Lunes 7:00-8:00, Martes 7:00-8:00",
        "type_place" : "Comida",
        "image" : "010101",
        "status" : 1
    }
    bear = "Bearer " + token
    data2 = json.dumps(data)
    headers = {"Content-Type": "application/json", "Authorization" : bear}
    response = connection.request(
        "POST",
        "/lugares",
        data2,
        headers,
    )
    generalResponse = connection.getresponse()
    statusCode = generalResponse.status
    assert HTTPStatus.CREATED == statusCode

def test_GetPlace():
    global token
    connection = httpClient.HTTPConnection("127.0.0.1",9000)
    bear = "Bearer " + token
    headers = {"Content-Type": "application/json", "Authorization" : bear}
    response = connection.request(
        "GET",
        "/lugares/1",
        "",
        headers
    )
    generalResponse = connection.getresponse()
    statusCode = generalResponse.status
    assert statusCode == HTTPStatus.OK

def test_DeletePlace():
    global token
    connection = httpClient.HTTPConnection("127.0.0.1",9000)
    bear = "Bearer " + token
    headers = {"Content-Type": "application/json","Authorization" : bear}
    response = connection.request(
        "DELETE",
        "/lugares/1",
        "",
        headers
    )
    generalResponse = connection.getresponse()
    statusCode = generalResponse.status
    assert statusCode == HTTPStatus.NO_CONTENT

def test_PostComment():
    global token
    connection = httpClient.HTTPConnection("127.0.0.1",9000)
    data = {
        "student": "zs19014022",
        "date": "25/05/2022",
        "hour" : "12:30",
        "description" : "Muy agradable la carrera",
        "forum_id" : 4
    }
    bear = "Bearer " + token
    data2 = json.dumps(data)
    headers = {"Content-Type": "application/json", "Authorization" : bear}
    response = connection.request(
        "POST",
        "/foros/4/comentarios",
        data2,
        headers,
    )
    generalResponse = connection.getresponse()
    statusCode = generalResponse.status
    assert HTTPStatus.OK == statusCode
    