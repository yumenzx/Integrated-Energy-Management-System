# imagine node.js
FROM node:14 AS build

# setari
WORKDIR /app

# copiere fisiere si dependinte
COPY ./package*.json ./
RUN npm install

# copiere proiect si build
COPY ./ ./
RUN npm run build

# 
FROM nginx:alpine

# 
COPY --from=build /app/dist /usr/share/nginx/html

# Expose the port
EXPOSE 80

# 
