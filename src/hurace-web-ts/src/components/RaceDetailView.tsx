import React from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';

const RacePanel = styled.div``;

export const RaceDetailView: React.FC<{ raceId: number }> = ({ raceId }) => {
    return (
        <RacePanel>
            <Link to="/seasons">Back To the underground</Link>
        </RacePanel>
    );
};
