# Build stage
FROM node:lts-alpine AS build
WORKDIR /app

# Copy package files and install
COPY presentation/package*.json ./
RUN npm install

# Copy source and config
COPY presentation/ ./
# We also need the shared config during build or at least in public
COPY app-config.json ./public/app-config.json

# Build the app
RUN npm run build

# Runtime stage
FROM nginx:stable-alpine
COPY --from=build /app/dist /usr/share/nginx/html
# Standard Nginx config
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
