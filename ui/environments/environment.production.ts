import { Environment } from './environment.interface';

export const environment: Environment = {
  production: true,
  // Set this to your production API URL
  // If using Azure Functions, use: '/api' for relative path
  // If using separate API service, use full URL: 'https://your-api.azurewebsites.net'
  apiUrl: '/api',
  // Add your Geoapify API key for production
  geoapifyApiKey: undefined
};

