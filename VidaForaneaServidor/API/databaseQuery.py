from http import HTTPStatus
import http.client as httpClient
import json
from time import sleep

from pyparsing import line

connection = httpClient.HTTPConnection("127.0.0.1",9090)
file = open("databaseData.txt", "r")
lines = file.read().splitlines()

def Login():
    global token
    global refresh_token
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

def addStudents():
    print(lines)
    data = {
        "name" : "",
        "enrollment": "",
        "password": "",
        "status": 1,
        "rol" : "",
        "degree" : ""
    }
    j = 0
    for i in range(4):
        data["name"] = lines[0 + j]
        data["enrollment"] = lines[1 + j]
        data["password"] = lines[2 + j]
        data["status"] = int(lines[3 + j])
        data["rol"] = lines[4 + j]
        data["degree"] = lines[5 + j]
        j += 6
        print(data)
        data2 = json.dumps(data)
        headers = {"Content-Type": "application/json"}
        response = connection.request(
            "POST",
            "/estudiantes",
            data2,
            headers,
        )
        generalResponse = connection.getresponse().read()

def addComments():
    global token
    data = {
        "student": "",
        "date": "",
        "hour" : "",
        "description" : "",
        "forum_id" : 4
    }
    bear = "Bearer " + token
    headers = {"Content-Type": "application/json", "Authorization" : bear}
    j = 24
    for i in range(1):
        data["student"] = lines[0 + j]
        data["date"] = lines[1 + j]
        data["hour"] = lines[2 + j]
        data["description"] = lines[3 + j]
        data["forum_id"] = lines[4 + j]
        data2 = json.dumps(data)
        response = connection.request(
           "POST",
           "/foros/4/comentarios",
           data2,
           headers,
        )
        generalResponse = connection.getresponse().read()
        

addStudents()
Login()
addComments()
    