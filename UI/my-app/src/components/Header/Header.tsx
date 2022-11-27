import { Avatar, Box, Button, Typography } from "@mui/material";
import React from "react";
import LogoutIcon from '@mui/icons-material/Logout';
import { listItemButton, buttonImageIconStyles } from "../../styles/Button/ButtonStyles";
import { useStore } from "../../api/stores/Store";
import { useLocation, useNavigate } from "react-router";

const headerBox = {
    bgcolor: "orange",
    mt: -1,
    ml: -1, 
    mr: -1,
    display: "grid",
    gridTemplateColumns: "6fr 1fr"
}

export default function Header() {
    const { userStore } = useStore();
    const navigate = useNavigate();
    const location = useLocation();

    const logout = () => {
        navigate('/logout', { replace: true });
    }

    const userProfile = () => {
        navigate('/userProfile', { replace: true });
    }

    return( 
        <>
            {
                !location.pathname.startsWith('/login') 
             && !location.pathname.startsWith('/login') 
             &&
                <Box sx={headerBox}>
                    <Typography  variant="h2" component="h2" sx={{ display: "inline-block"}}>
                        Pet Helper
                    </Typography>
                        <Box sx={{ display: "flex", justifyContent: "center", mt: 2}}>
                        <Avatar sx={listItemButton} onClick={userProfile}>
                            {userStore.user?.firstName[0].toUpperCase()}
                            {userStore.user?.lastName[0].toUpperCase()}
                        </Avatar>
                        <Button sx={{...listItemButton, width: "20px", height: "40px"}}
                                onClick={logout}
                        >
                            <LogoutIcon />
                        </Button>
                    </Box>
                </Box>
            }
        </>
    );
}