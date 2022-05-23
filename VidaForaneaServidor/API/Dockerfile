FROM python:3.8-slim-buster

EXPOSE 9000/tcp
EXPOSE 9000/udp
WORKDIR /api_vidaforanea

COPY requirements.txt requirements.txt
RUN pip3 install -r requirements.txt

COPY . .

CMD [ "python3", "-m" , "flask", "run", "--host=0.0.0.0"]