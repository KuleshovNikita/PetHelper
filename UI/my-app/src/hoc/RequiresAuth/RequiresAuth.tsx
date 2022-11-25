import {Navigate, useLocation} from "react-router-dom";
import { useStore } from "../../api/stores/Store";

type Props = {
    children: JSX.Element
}

export const RequireAuth = ({children}: Props) => {
    const location = useLocation();
    const { userStore } = useStore();

    if (!userStore.isLoggedIn) {
        return <Navigate to='/login' state={{from: location}}/>
    }

    return children;
}
