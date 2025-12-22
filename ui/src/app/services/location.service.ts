import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, from } from 'rxjs';
import { map, catchError, debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { environment } from 'environments/environment';

export interface LocationCoordinates {
  latitude: number;
  longitude: number;
}

export interface GeoapifyAutocompleteResult {
  properties: {
    formatted: string;
    lat: number;
    lon: number;
    city?: string;
    state?: string;
    country?: string;
  };
}

export interface GeoapifyAutocompleteResponse {
  features: GeoapifyAutocompleteResult[];
}

export interface GeoapifyGeocodeResponse {
  results: Array<{
    lat: number;
    lon: number;
    formatted: string;
  }>;
}

export interface AddressSuggestion {
  address: string;
  latitude: number;
  longitude: number;
  city?: string;
  state?: string;
  country?: string;
}

@Injectable({ providedIn: 'root' })
export class LocationService {
  private readonly http = inject(HttpClient);
  private readonly geoapifyApiKey = environment.geoapifyApiKey || '';

  /**
   * Get user's current location using browser geolocation API
   */
  public getCurrentLocation(): Observable<LocationCoordinates> {
    return new Observable(observer => {
      if (!navigator.geolocation) {
        observer.error('Geolocation is not supported by this browser.');
        return;
      }

      navigator.geolocation.getCurrentPosition(
        (position) => {
          observer.next({
            latitude: position.coords.latitude,
            longitude: position.coords.longitude
          });
          observer.complete();
        },
        (error) => {
          console.error('Error getting location:', error);
          // Fallback to default location (Bangalore)
          observer.next({
            latitude: 12.9716,
            longitude: 77.5946
          });
          observer.complete();
        },
        {
          enableHighAccuracy: true,
          timeout: 10000,
          maximumAge: 0
        }
      );
    });
  }

  /**
   * Get address autocomplete suggestions using Geoapify Autocomplete API
   */
  public getAddressSuggestions(query: string): Observable<AddressSuggestion[]> {
    if (!query || query.length < 2) {
      return of([]);
    }

    if (!this.geoapifyApiKey) {
      console.warn('Geoapify API key not configured');
      return of([]);
    }

    const params = new HttpParams()
      .set('text', query)
      .set('apiKey', this.geoapifyApiKey)
      .set('limit', '5')
      .set('type', 'amenity,street,locality,district,city');

    return this.http.get<GeoapifyAutocompleteResponse>('https://api.geoapify.com/v1/geocode/autocomplete', { params }).pipe(
      map((response) => {
        if (response.features && response.features.length > 0) {
          return response.features.map((feature) => ({
            address: feature.properties.formatted,
            latitude: feature.properties.lat,
            longitude: feature.properties.lon,
            city: feature.properties.city,
            state: feature.properties.state,
            country: feature.properties.country
          }));
        }
        return [];
      }),
      catchError((error) => {
        console.error('Error getting address suggestions:', error);
        return of([]);
      })
    );
  }

  /**
   * Get coordinates from address using Geoapify Geocoding API
   */
  public getCoordinatesFromAddress(address: string): Observable<LocationCoordinates> {
    if (!this.geoapifyApiKey) {
      console.warn('Geoapify API key not configured, using fallback location');
      return of({
        latitude: 12.9716,
        longitude: 77.5946
      });
    }

    const params = new HttpParams()
      .set('text', address)
      .set('apiKey', this.geoapifyApiKey)
      .set('limit', '1');

    return this.http.get<GeoapifyGeocodeResponse>('https://api.geoapify.com/v1/geocode/search', { params }).pipe(
      map((response) => {
        if (response.results && response.results.length > 0) {
          return {
            latitude: response.results[0].lat,
            longitude: response.results[0].lon
          };
        }
        throw new Error('No coordinates found');
      }),
      catchError((error) => {
        console.error('Error getting coordinates from address:', error);
        // Fallback to default location (Bangalore)
        return of({
          latitude: 12.9716,
          longitude: 77.5946
        });
      })
    );
  }
}

