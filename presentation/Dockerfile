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

EXPOSE 80
CMD printf '{"Backend":{"Port":%s,"Url":"%s"},"Frontend":{"Port":%s,"Url":"%s"}}' "${BACKEND_PORT:-61471}" "${BACKEND_URL:-http://localhost:61471}" "${FRONTEND_PORT:-5173}" "${FRONTEND_URL:-http://localhost:5173}" > /usr/share/nginx/html/app-config.json && nginx -g 'daemon off;'
