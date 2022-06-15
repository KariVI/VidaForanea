FROM python:3.8-slim-buster

EXPOSE 9000/tcp
EXPOSE 9000/udp
WORKDIR /api_vidaforanea:v2

COPY ./VidaForaneaServidor/API/requirements.txt ./VidaForaneaServidor/API/requirements.txt
RUN pip3 install -r ./VidaForaneaServidor/API/requirements.txt

COPY . .

CMD [ "python3", "-m" , "flask", "run", "--host=0.0.0.0"]