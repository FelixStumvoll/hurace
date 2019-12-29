import React from 'react';
import styled from 'styled-components';
import { LocationViewItem } from './LocationViewItem';
import {useDispatch, useSelector} from 'react-redux';

import {StoreState} from "../../store/rootReducer";

const LocationPanel = styled.div``;

const LocationItemPanel = styled.div`
    display: flex;
    flex-wrap: wrap;
`;

const LocationLabel = styled.div`
    font-weight: bold;
    font-size: 20px;
    margin-bottom: 10px;
`;

export const LocationView: React.FC<{ seasonId: number }> = ({ seasonId }) => {
    let locations = useSelector((state: StoreState) => state.locations.locations );
    let season = useSelector((state: StoreState) => state.seasons.seasons.find(s => s.id == seasonId));
    console.log(season);
    let seasonDescription;
    if(season === undefined){
            seasonDescription = <div/>;
    }else{
        seasonDescription = <LocationLabel>{season.startDate.getFullYear()}-{season.endDate.getFullYear()} ></LocationLabel>;
    }
    return (
        <LocationPanel>
            {seasonDescription}
            <LocationItemPanel>
                {locations.map(l => (
                    <LocationViewItem key={l.id} location={l} />
                ))}
            </LocationItemPanel>
        </LocationPanel>
    );
};
