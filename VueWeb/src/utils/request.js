import axios from 'axios'
import notification from 'ant-design-vue/es/notification'
import { VueAxios } from './axios'
import { errorMessageMap } from './errorMsg'

// 创建 axios 实例
const request = axios.create({
  // API 请求的默认前缀
  baseURL: process.env.VUE_APP_API_BASE_URL,
  timeout: 60000, // 请求超时时间
})

// 异常拦截处理器
const errorHandler = (error) => {
  if (error.response) {
    const data = error.response.data
    // 从 localstorage 获取 token
    if (error.response.status === 403) {
      notification.error({
        message: '请求被禁止',
        description: data.message,
      })
    }
  }
  return Promise.reject(error)
}

// request interceptor
request.interceptors.request.use((config) => {
  return config
}, errorHandler)

// response interceptor
request.interceptors.response.use((response) => {
  if (response.data.status && response.data.status !== 'Success') {
    const errorMsg = errorMessageMap[response.data.errorMessage] || `请求失败-${response.data.errorMessage}`
    errorMsg &&
      notification.error({
        message: '错误',
        description: errorMsg,
      })
  }
  if (response.config.useOriginalReponse) return response
  else return response.data
}, errorHandler)

const installer = {
  vm: {},
  install(Vue) {
    Vue.use(VueAxios, request)
  },
}

export default request

export { installer as VueAxios, request as axios }
