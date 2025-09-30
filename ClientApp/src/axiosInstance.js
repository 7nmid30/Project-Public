import axios from "axios";

const axiosInstance = axios.create({
    baseURL: "/api",
    headers: {
        "Content-Type": "application/json"
    }
});

// 認証トークンをヘッダーに追加するインターセプター
axiosInstance.interceptors.request.use(config => {
    const token = localStorage.getItem("token"); //ブラウザのローカルストレージからトークンを取得
    if (token) {
        config.headers.Authorization = `Bearer ${token}`; //リクエストヘッダーに認証トークンを追加
    }
    return config;
}, error => {
    return Promise.reject(error);
});

export default axiosInstance;
