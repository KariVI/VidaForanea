
from http import HTTPStatus
import http.client as httpClient
import json

def test_PostStudent():
    connection = httpClient.HTTPConnection("127.0.0.1",9090)
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


def test_Login():
    global token
    global refresh_token
    connection = httpClient.HTTPConnection("127.0.0.1",9090)
    data = {
        "enrollment": "zs19014020",
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
    connection = httpClient.HTTPConnection("127.0.0.1",9090)
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
    connection = httpClient.HTTPConnection("127.0.0.1",9090)
    headers = {"Content-Type": "application/json"}
    response = connection.request(
        "GET",
        "/estudiantes/zs19014020",
        "",
        headers
    )
    generalResponse = connection.getresponse()
    statusCode = generalResponse.status
    assert statusCode == HTTPStatus.OK
