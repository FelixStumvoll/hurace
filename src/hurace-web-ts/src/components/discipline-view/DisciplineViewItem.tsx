import React from 'react';
import { Discipline } from '../../interfaces/Discipline';
import styled from 'styled-components';
import { StoreState } from '../../store/rootReducer';
import { useSelector } from 'react-redux';
import { RaceViewItem } from '../race-view/RaceViewItem';

const DisciplinePanel = styled.div`
    margin-bottom: 10px;
    padding: 5px;
`;

const DisciplineLabel = styled.div`
    font-weight: bold;
    font-size: 20px;
    margin-bottom: 5px;
`;

const RacesPanel = styled.div`
    display: flex;
    flex-wrap: wrap;
`;

export const DisciplineViewItem: React.FC<{ discipline: Discipline }> = ({
    discipline
}) => {
    const races = useSelector((state: StoreState) => state.races.races);

    return (
        <DisciplinePanel>
            <DisciplineLabel>{discipline.disciplineName}</DisciplineLabel>
            <RacesPanel>
                {races.map(r => (
                    <RaceViewItem key={r.id} race={r} />
                ))}
            </RacesPanel>
        </DisciplinePanel>
    );
};
