import { ListItem, ListItemButton, ListItemText, Typography } from "@mui/material";
import { info } from "console";
import React, { useState } from "react";
import { useNavigate } from "react-router";
import { AnimalType, Pet } from "../../models/Pet";
import PetListItem from "./PetListItem";

type Props = {
    pets: Pet[]
}

const listItemStyle = {
    display: "block",
    bgcolor: "orange",
    borderRadius: "5px",
    mb: 1
}

export default function PetsList({ pets }: Props) {
    const [petList, setPetList] = useState(pets);

    const removePet = (pet: Pet) => {
        setPetList(petList.filter(p => p != pet));
    }

    return(
        <>
        <Typography variant="h4" component="h4">
            Pets:
        </Typography>
            {
                petList && 
                petList.length !== 0 &&
                petList.map((pet, key) => (
                    <ListItem key={key} component="div" sx={listItemStyle}>
                        <PetListItem petItem={pet} removeItem={() => removePet(pet)}/>
                    </ListItem>
                ))
            }
        </>
    );
}