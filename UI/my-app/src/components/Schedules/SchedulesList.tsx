import { Button, ListItem, ListItemButton, ListItemText, Typography } from "@mui/material";
import React, { Component } from "react";
import { WalkingSchedule } from "../../models/Pet";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';

type Props = {
    schedules: WalkingSchedule[]
}

const listItemStyle = {
    bgcolor: "orange",
    borderRadius: "5px",
    mb: 1
}

const listItemChangeButton = {
    bgcolor: "white",
    color: "orange",
    mr: 1,
    "&:hover": {
        bgcolor: "brown"
    }
}

export default function SchedulesList({ schedules }: Props) {
    const normalizeTime = (schedule: WalkingSchedule) => {
        const normStart = new Date(schedule.scheduledStart).toLocaleTimeString();
        const normEnd = new Date(schedule.scheduledEnd).toLocaleTimeString();

        return (
            <Typography 
                variant="h4" 
                component="h4"
                color="white">
                {`${normStart} - ${normEnd}`}
            </Typography>
        );
    }

    return(
        <>
            <Typography variant="h4" component="h4">
                Schedules:
            </Typography>
                {
                    schedules && 
                    schedules.length !== 0 &&
                    schedules.map((sch, key) => (
                        <ListItem key={key} component="a" sx={listItemStyle}>
                            <ListItemText primary={normalizeTime(sch)} />
                            <Button sx={listItemChangeButton}><EditIcon/></Button>
                            <Button sx={listItemChangeButton}><DeleteIcon/></Button>
                        </ListItem>
                    ))
                }
        </>
    );
}