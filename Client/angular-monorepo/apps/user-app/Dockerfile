FROM node:20.17.0-alpine AS build

WORKDIR /app

COPY package*.json ./

RUN npm install

RUN npx ngcc --properties es2023 browser module main --first-only --create-ivy-entry-points

COPY . .

RUN npx nx build user-app --base-href /user/


FROM nginx:stable

COPY default.conf /etc/nginx/conf.d

COPY --from=build /app/dist/apps/user-app/browser /usr/share/nginx/html

EXPOSE 80
