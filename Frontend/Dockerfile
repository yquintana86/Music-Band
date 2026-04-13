# Build dependencies
FROM node:22-alpine AS base

WORKDIR /app

COPY package*.json ./

RUN npm install

# Build stage
FROM node:22-alpine AS build

WORKDIR /app

COPY --from=base /app/node_modules ./node_modules

COPY . .

RUN npx ng build --configuration production \
    --output-path=dist/music-band \
    --base-href=/ \
    --deploy-url=/


# Runtime stage
FROM nginx:alpine

COPY --from=build /app/dist/music-band/browser /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
