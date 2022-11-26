import { Route, Routes, Navigate } from "react-router";
import NotFound from "../../pages/NotFound/NotFound";
import { Login } from "../../pages/Login/Login";
import { RequireAuth } from "../../hoc/RequiresAuth/RequiresAuth";
import UserProfile from "../../pages/UserProfile/UserProfile";
import Registration from "../../pages/Registration/Registration";
import { useStore } from "../../api/stores/Store";

export function AppRoutes() {
    return (
        <Routes>
            <Route path="" element={ <Navigate to="/userProfile"/>} />
            <Route path="/login" element={<Login/>} />
            <Route path="/logout" element={<Logout/>} />
            <Route path="/userProfile" element={<RequireAuth><UserProfile/></RequireAuth>} />
            <Route path="/register" element={ <Registration /> } />
            <Route path="/notFound" element={ <NotFound /> } />
            <Route path="*" element={<Navigate to="/notFound" replace />} />
        </Routes>
    );
}

function Logout() {
    const { userStore } = useStore();

    userStore.logout();

    return (
        <Navigate to="/login" replace={true}/>
    )
}