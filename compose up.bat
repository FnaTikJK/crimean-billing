echo on
IF NOT EXIST ".env" (copy docker-local-env .env)
docker-compose up
@echo off
pause