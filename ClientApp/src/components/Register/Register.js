import React, { useState } from "react";
import axios from "axios"; //複雑な認証フローやトークンの管理が必要な場合や、再利用可能なリクエスト設定が必要な場合は、axiosのほうが便利
import axiosInstance from "../../axiosInstance";
import { Link } from "react-router-dom"; // React Routerを利用する場合

function Register() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const handleRegister = async (e) => {
        e.preventDefault();
        try {
            const response = await axiosInstance.post("/auth/register", { email, password });
            alert("Registration successful!");
        } catch (error) {
            console.error("Registration failed:", error);
            alert("Failed to register. Try again.");
        }
    };

    return (
        <form className="login-form" onSubmit={handleRegister}>
            <h2 className="login-form-title">アカウントの作成</h2>
            <input type="email" className="form-input" value={email} onChange={(e) => setEmail(e.target.value)} placeholder="Email" required />
            <input type="password" className="form-input" value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Password" required />
            <button type="submit" className="form-button">アカウントを作成する</button>
            <p className="form-footer">
                既存ユーザーですか？ <Link to="/login" className="form-link">ログイン</Link>
            </p>
        </form>
    );
}

export default Register;
