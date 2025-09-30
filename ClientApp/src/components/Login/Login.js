import React, { useState } from "react";
import { Link } from "react-router-dom"; // React Routerを利用する場合
import axios from "axios";
import './Login.css';

function Login() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.post("/api/auth/login", { email, password });//ASP.NET Coreでは、Web API のエンドポイントを他のエンドポイント（例: HTMLやCSSの提供用）と区別するために、api/ をURLに付加するのが一般的
            localStorage.setItem("token", response.data.token);
            alert("Login successful!");
            window.location.href = '/top';
        } catch (error) {
            console.error("Login failed:", error);
            alert("Invalid credentials");
        }
    };

    return (
        <form className="login-form" onSubmit={handleLogin}>
            <h2 className="login-form-title">ログイン</h2>
            <input type="email" className="form-input" value={email} onChange={(e) => setEmail(e.target.value)} placeholder="Email" required />
            <input type="password" className="form-input" value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Password" required />
            <button type="submit" className="form-button">ログインする</button>
            <p className="form-footer">
                アカウントが未登録ですか？ <Link to="/register" className="form-link">アカウントの作成</Link>
            </p>
        </form>
    );
}

export default Login;
