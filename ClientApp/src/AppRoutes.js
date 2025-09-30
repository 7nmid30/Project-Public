import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import Top from "./components/Top"; //default‚Åexport‚³‚ê‚Ä‚é‚Ì‚ÅTop‚¶‚á‚È‚­‚ÄˆÙ‚È‚é–¼‘O‚Å‚à‚¢‚¢
import Login from "./components/Login/Login";
import Register from "./components/Register/Register";

const AppRoutes = [
    {
        index: true,
        element: <Home />
    },
    {
        path: '/counter',
        element: <Counter />
    },
    {
        path: '/fetch-data',
        element: <FetchData />
    },
    {
        path: '/top',
        element: <Top />
    },
    {
        path: '/login',
        element: <Login />
    },
    {
        path: '/register',
        element: <Register />
    }
];

export default AppRoutes;
