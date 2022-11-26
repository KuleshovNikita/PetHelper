import { deepOrange } from "@mui/material/colors"

export const mainBoxStyle = {
    ml: 3,
    mt: 2,
    mr: 3,
    display: "grid",
    gridTemplateColumns: "1fr 4fr"     
}

export const profileBoxStyle = { 
    "&": {
        display: "flex",
        flexDirection: "column",
        alignItems: "left",
        width: 260
    },
}

export const avatarStyle = { 
    width: 240, 
    height: 240,
    fontSize: 100,
    mb: 2,
    bgcolor: deepOrange[500]
}

export const profileButtonsBoxStyles = { 
    display: "flex", 
    justifyContent: "space-between", 
    width: 260 
}

export const profileButtonHoverStyles = { 
    "&:hover": { 
        bgcolor: "orange" 
    }, 
    mt: 1 
}

export const buttonImageIconStyles = { 
    ml: 1, 
    bgcolor: "white", 
    color: "blue" 
}

