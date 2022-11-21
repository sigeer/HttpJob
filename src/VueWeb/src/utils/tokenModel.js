import { ACCESS_TOKEN } from '@/store/mutation-types'
import storage from 'store'

export class TokenModel {
  constructor(json) {
    json = json || {}
    if (typeof json === 'string' && json.startsWith('{')) {
      if (json.startsWith('{'))
        json = JSON.parse(json)
      else
        json = {}
    }
    this.access_token = json.access_token || ''
    this.refresh_token = json.refresh_token || ''
    this.time_expired = null
    if (new Date(json.time_expired) != 'Invalid Date') this.time_expired = new Date(json.time_expired)
  }

  toString() {
      return JSON.stringify(this)
  }

  get AccessToken() {
    if (this.isValid)
      return this.access_token;
    else
      return null;
  }

  get RefreshToken() {
    if (this.refresh_token)
      return this.refresh_token;
    else
      return null;
  }

  get isValid() {
    if (this.time_expired === null || this.time_expired <= new Date()) {
      this.access_token = ''
    }
    return this.access_token !== ''
  }
}

export function saveToken(token) {
  let t = new TokenModel()
  if (typeof token === 'string') {
    if (token.startsWith('{')) {
      t = JSON.parse(token)
    }
  } else {
    t = token
  }
  const time = new Date(new Date().getTime() + t.expires_in * 1000)
  const m = new TokenModel({
    access_token: t.access_token,
    refresh_token: t.refresh_token,
    time_expired: time,
  })
  if (m.isValid) {
    storage.set(ACCESS_TOKEN, JSON.stringify(m))
  }
  return m
}

export function getToken() {
  let data = storage.get(ACCESS_TOKEN)
  const model = new TokenModel(data)
  return model
}

