import { Button, ListItem, Typography } from "@mui/material";
import React, { useState } from "react";
import { Pet } from "../../models/Pet";
import PetListItem from "./PetListItem";
import AddIcon from '@mui/icons-material/Add';
import { useNavigate } from "react-router";

type Props = {
    pets: Pet[]
}

const listItemStyle = {
    display: "block",
    bgcolor: "orange",
    borderRadius: "5px",
    mb: 1
}

const addPetButton = {
    "&:hover": {
        bgcolor: "brown"
    },
    bgcolor: "orange", 
    color: "white"
}

export default function PetsList({ pets }: Props) {
    const [petList, setPetList] = useState(pets);
    const navigate = useNavigate();

    const removePet = (pet: Pet) => {
        setPetList(petList.filter(p => p != pet));
    }

    const openPetProfile = () => {
        navigate('/pet/new/false', { replace: true });
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

            <Button sx={addPetButton} onClick={openPetProfile}>
                <AddIcon/>
            </Button>
        </>
    );
}