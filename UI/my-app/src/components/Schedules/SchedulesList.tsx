import { Button, ListItem, ListItemButton, ListItemText, TextField, Typography } from "@mui/material";
import React, { Component, useState } from "react";
import { Pet, WalkingSchedule } from "../../models/Pet";
import AddIcon from '@mui/icons-material/Add';
import ScheduleItem from "./ScheduleItem";

type Props = {
    schedules: WalkingSchedule[],
    pet: Pet,
}

const listItemStyle = {
    bgcolor: "orange",
    borderRadius: "5px",
    mb: 1
}

const addScheduleButton = {
    "&:hover": {
        bgcolor: "brown"
    },
    bgcolor: "orange", 
    color: "white"
}

export default function SchedulesList({ schedules, pet }: Props) {
    const [scheduleItems, setScheduleItems] = useState(schedules);

    const addNewSchedule = () => {
        const newSchedule: WalkingSchedule = {
            scheduledStart: '',
            scheduledEnd: '',
            petId: pet.id,
            pet: pet,
            id: ""
        };

        setScheduleItems((items) => [...items, newSchedule]);
    }

    const removeSchedule = (schedule: WalkingSchedule) => {
        setScheduleItems(scheduleItems.filter(sc => sc != schedule));
    }

    return(
        <>
            <Typography variant="h4" component="h4">
                Schedules:
            </Typography>
                {
                    scheduleItems && 
                    scheduleItems.length !== 0 &&
                    scheduleItems.map((sch, key) => (
                        <ListItem key={key} component="a" sx={listItemStyle}>
                            <ScheduleItem scheduleItem={sch} removeItem={() => removeSchedule(sch)}/>
                        </ListItem>
                    ))
                }
                
            <Button sx={addScheduleButton}
                    onClick={addNewSchedule}>
                <AddIcon/>
            </Button>
        </>
    );
}