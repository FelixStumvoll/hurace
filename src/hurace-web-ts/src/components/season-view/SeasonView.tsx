import React from 'react';
import { SeasonViewItem } from './SeasonViewItem';
import styled from 'styled-components';
import { useDispatch, useSelector } from 'react-redux';
import {StoreState} from "../../store/rootReducer";

const SeasonItemPanel = styled.div`
    display: flex;
    flex-direction: column;
    flex-wrap: wrap;
`;

const SeasonLabel = styled.div`
    font-weight: bold;
    font-size: 20px;
    margin-bottom: 10px;
`;

export const SeasonView: React.FC = () => {
    let seasons = useSelector((state: StoreState) => state.seasons.seasons);

    return (
        <div>
            <SeasonLabel>Alle Saisonen:</SeasonLabel>
            <SeasonItemPanel>
                {seasons.map(s => (
                    <SeasonViewItem key={s.id} season={s} />
                ))}
            </SeasonItemPanel>
        </div>
    );
};
